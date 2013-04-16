using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBCommService;
using Game.Sprites;
using Game.States;
using Engine;


namespace Game.Quests
{
    /// <summary>
    /// Performs all tasks to do with Quests. This includes Quest checking and scheduling. 
    /// </summary>
    static class QuestChecking
    {
        /// <summary>
        /// List of all the Quests 
        /// </summary>
        public static List<Quest> AllQuests;

        /// <summary>
        /// Checks whether the user has completed a quest. Checks all QuestNodeTypes except for Use
        /// </summary>
        public static void PerformCheck()
        {
            if (AreaState.character.Quests.Count > 0)
            {
                if (AreaState.character.Quests[0].current_node != AreaState.character.Quests[0].num_nodes)
                {
                    switch (AreaState.character.Quests[0].Nodes[AreaState.character.Quests[0].current_node].NodeType)
                    {
                        case QuestNodeType.Goto:
                            if (AreaState.character.position == AreaState.character.Quests[0].Nodes[AreaState.character.Quests[0].current_node].NodeLocation)
                            {
                                TriggerNodeCompleted();
                            }
                            break;
                        case QuestNodeType.Energy:
                            if (AreaState.character.Quests[0].Nodes[AreaState.character.Quests[0].current_node].accumulated_value
                                + AreaState.character.Quests[0].Nodes[AreaState.character.Quests[0].current_node].NodeValue >= AreaState.character.energy
                                || AreaState.character.energy == 100)
                            {
                                TriggerNodeCompleted();
                            }
                            break;
                        case QuestNodeType.Money:
                            if (AreaState.character.Quests[0].Nodes[AreaState.character.Quests[0].current_node].accumulated_value
                                + AreaState.character.Quests[0].Nodes[AreaState.character.Quests[0].current_node].NodeValue >= AreaState.character.money)
                            {
                                TriggerNodeCompleted();
                            }
                            break;
                        case QuestNodeType.Sanity:
                            if (AreaState.character.Quests[0].Nodes[AreaState.character.Quests[0].current_node].accumulated_value
                                + AreaState.character.Quests[0].Nodes[AreaState.character.Quests[0].current_node].NodeValue >= AreaState.character.sanity
                                || AreaState.character.sanity == 100)
                            {
                                TriggerNodeCompleted();
                            }
                            break;
                        case QuestNodeType.Bladder:
                            if (AreaState.character.Quests[0].Nodes[AreaState.character.Quests[0].current_node].accumulated_value
                                + AreaState.character.Quests[0].Nodes[AreaState.character.Quests[0].current_node].NodeValue >= AreaState.character.bladder
                                || AreaState.character.bladder == 100)
                            {
                                TriggerNodeCompleted();
                            }
                            break;
                        case QuestNodeType.Use:
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Checks whether the user is using an item in a specific area
        /// </summary>
        public static void UsingItem()
        {
            if (AreaState.character.Quests.Count > 0)
            {
                if (AreaState.character.Quests[0].current_node != AreaState.character.Quests[0].num_nodes)
                {
                    if (AreaState.character.position == AreaState.character.Quests[0].Nodes[AreaState.character.Quests[0].current_node].NodeLocation)
                    {
                        TriggerNodeCompleted();
                        AreaState.character.money += 25;
                    }
                }
            }
        }

        /// <summary>
        /// Increments current_node and updates the next node and the database.
        /// </summary>
        private static void TriggerNodeCompleted()
        {
            if (AreaState.character.Quests.Count > 0)
            {
                AreaState.character.Quests[0].current_node++;
                if (AreaState.character.Quests[0].current_node < AreaState.character.Quests[0].num_nodes)
                {
                    SetNodeValue(AreaState.character.Quests[0].Nodes[AreaState.character.Quests[0].current_node]);
                }
                else
                {
                    TPEngine.Get().State.PushState(new CongratzState(), true);
                }
                UpdateQuest(AreaState.character, AreaState.character.Quests[0]);
                ((CompletedNodeSprite)TPEngine.Get().SpriteDictionary["completedNodeSprite"]).IsItAlive = true;
            }
        }


        /// <summary>
        /// Adds a quest association into the database
        /// </summary>
        /// <param name="character">the character object</param>
        /// <param name="quest">the quest object</param>
        private static void UpdateQuest(Character character, Quest quest)
        {
            using (DBCommServiceClient srv = new DBCommServiceClient())
            {
                srv.UpdateQuestAssociation(GameLauncher_LoginButtonSprite.getUser(), character, quest);
            }
        }

        /// <summary>
        /// Checks for new quests
        /// Called when a new day has occured in the game. 
        /// </summary>
        public static void SignalNewQuestDay()
        {
            if (!CorrectQuestAssignedCheck())
            {
                foreach (Quest quest in AllQuests)
                {
                    if (quest.day_assigned == AreaState.character.day + ((AreaState.character.week - 1) * 5))
                    {
                        AreaState.character.Quests = CreateNewQuestList(quest);
                    }
                }
            }
        }

        /// <summary>
        /// Checks if the currently assigned Quests in the character are valid
        /// </summary>
        /// <returns>true if the correct quest is assigned</returns>
        private static bool CorrectQuestAssignedCheck()
        {
            bool result = false;
            if (AreaState.character.Quests.Count > 0)
            {
                result = AreaState.character.Quests[0].day_assigned == AreaState.character.day + ((AreaState.character.week - 1) * 5);
            }
            return result;
        }

        /// <summary>
        /// Creates a new quest list for character.Quests.
        /// </summary>
        /// <param name="quest">the new quest</param>
        /// <returns>the new quest list</returns>
        private static List<Quest> CreateNewQuestList(Quest quest)
        {
            List<Quest> newQuestList = new List<Quest>();

            SetNodeValue(quest.Nodes[0]);

            newQuestList.Add(quest);

            using (DBCommServiceClient srv = new DBCommServiceClient())
            {
                srv.SaveQuestAssociation(GameLauncher_LoginButtonSprite.getUser(), AreaState.character, quest);
            }

            return newQuestList;
        }

        /// <summary>
        /// Checks what type the param QuestNode is and assigns the correct value to the node.
        /// </summary>
        /// <param name="theNewQuest">the quest node to check</param>
        private static void SetNodeValue(QuestNode theNewQuest)
        {
            switch (theNewQuest.NodeType)
            {
                case QuestNodeType.Goto:
                    theNewQuest.accumulated_value = 0;
                    break;
                case QuestNodeType.Energy:
                    theNewQuest.accumulated_value = (int)AreaState.character.energy;
                    break;
                case QuestNodeType.Money:
                    theNewQuest.accumulated_value = (int)AreaState.character.money;
                    break;
                case QuestNodeType.Sanity:
                    theNewQuest.accumulated_value = (int)AreaState.character.sanity;
                    break;
                case QuestNodeType.Bladder:
                    theNewQuest.accumulated_value = (int)AreaState.character.bladder;
                    break;
                case QuestNodeType.Use:
                    theNewQuest.accumulated_value = 0;
                    break;
            }
        }

        /// <summary>
        /// Retrieves all Quests from service
        /// </summary>
        public static void GetQuests()
        {
            using (DBCommServiceClient svr = new DBCommServiceClient())
            {
                AllQuests = svr.LoadQuestData(GameLauncher_LoginButtonSprite.getUser());
            }
        }
    }
}
