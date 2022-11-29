﻿using PupUp.Data;
using PupUp.Models.Quests;
using PupUp.Models.Quests.Enums;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace PupUp.Services
{
    public class QuestService
    {
        private class QuestPair
        {
            public Quest Quest { get; set; }
            public List<Quest> Children { get; set; }
        }
        private readonly PupUpDbContext m_context;
        private readonly BadgeService m_badgeService;
        private readonly PointService m_pointService;
        private Dictionary<ActionType, ConcurrentBag<QuestPair>> m_questByAction = new Dictionary<ActionType, ConcurrentBag<QuestPair>>();
        public QuestService(PupUpDbContext context, BadgeService badgeService, PointService pointService)
        {
            m_context = context;
            m_badgeService = badgeService;
            m_pointService = pointService;
            foreach (ActionType action in Enum.GetValues<ActionType>())
            {
                m_questByAction.Add(action, new ConcurrentBag<QuestPair>());
            }
            var quests = m_context.Quests;
            QuestPair pair;
            foreach (var quest in quests)
            {
                m_questByAction[quest.ActionType].Add(pair = new QuestPair
                {
                    Quest = quest,
                    Children = new List<Quest>()
                });
                foreach (var questChild in quests)
                {
                    if (questChild.RequiredQuestIds.Contains(pair.Quest.Id))
                    {
                        pair.Children.Add(questChild);
                    }
                }
            }
        }
        public IEnumerable<Quest> Quests => m_context.Quests;
        public ICollection<UserQuest> GetQuests(string userId) => m_context.UserQuests.Where(q => q.UserId == userId).ToList();
        public async void DoAction(ActionType action, string userId, int? trainingId = null)
        {
            var quests = m_questByAction[action];
            var userQuests = GetQuests(userId);
            foreach (var pair in quests)
            {
                switch (action)
                {
                    case ActionType.StartTraining:
                    case ActionType.LearnTraning:
                    case ActionType.SkillTraining:
                        if (trainingId.HasValue && pair.Quest.ListenTrainingIds.Count > 0 && pair.Quest.ListenTrainingIds.Contains(trainingId.Value))
                            continue;
                        break;
                    default:
                        break;
                }
                var questIds = pair.Quest.RequiredQuestIds;
                if (questIds.Count > 0 && !questIds.All(id => userQuests.Any(q => q.QuestId == id)))
                    continue;

                var userQuest = userQuests.FirstOrDefault(q => q.QuestId == pair.Quest.Id);
                if (userQuest == null || pair.Quest.Repetable)
                {
                    if (userQuest == null)
                        m_context.UserQuests.Add(userQuest = new UserQuest()
                        {
                            QuestId = pair.Quest.Id,
                            Quest = pair.Quest,
                            UserId = userId
                        });

                    userQuest.State = QuestState.Completed;

                    #region Reward
                    switch (pair.Quest.RewardType)
                    {
                        case RewardType.Badget:
                            m_badgeService.AddBadges(pair.Quest.RewardIds, userId);
                            break;
                        case RewardType.Exp:
                            m_pointService.AddExp(userId, pair.Quest.RewardNum);
                            break;
                        case RewardType.Coin:
                            m_pointService.AddCoin(userId, pair.Quest.RewardNum);
                            break;
                        default:
                            break;
                    }
                    #endregion

                    foreach (var child in pair.Children)
                    {
                        if (child.RequiredQuestIds.All(id => userQuests.Any(q => q.QuestId == id)))
                        {
                            if (!m_context.UserQuests.Any(q => q.UserId == userId && q.QuestId == child.Id))
                            {
                                m_context.UserQuests.Add(new UserQuest()
                                {
                                    Quest = child,
                                    QuestId = child.Id,
                                    State = QuestState.Progress,
                                    UserId = userId
                                });
                            }
                        }
                    }
                }
            }
            await m_context.SaveChangesAsync();
        }
    }
}