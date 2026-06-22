using System.Collections.Generic;

public class GlobalInfo
{
    private static GlobalInfo _instance;
    private readonly Dictionary<string, string> npcDialogDictionary = new();
    private readonly Dictionary<string, string> npcDialogHeaderDictionary = new();

    public GlobalInfo()
    {
        // seed with data ...
        npcDialogHeaderDictionary.Add("Temple_Guard1", "Temple Guard 1");
        npcDialogHeaderDictionary.Add("Temple_Guard2", "Temple Guard 2");
        // by-stander ...
        npcDialogHeaderDictionary.Add("ByStander_01", "By-stander");
        // festival-attendees ..
        npcDialogHeaderDictionary.Add("FestivalAttendant_01", "Festival Attendant");

        // game object - press F to pay respects lol ...
        npcDialogHeaderDictionary.Add("AbolutionShrine_01", "A Shrine");
        // inspect game object - press F to inspect ..
        npcDialogHeaderDictionary.Add("EmaGameItem", "Ema Wooden plagues");
        // haiden monk 
        npcDialogHeaderDictionary.Add("HaidenMonk_01", "Monk");
        // haiden monk 
        npcDialogHeaderDictionary.Add("HaidenMonk_02", "Wandering Monk");
        // watching the play or performance
        npcDialogHeaderDictionary.Add("KaguraDenGameObjectIteraction", "Kagura Den");
        // shrine
        npcDialogHeaderDictionary.Add("HaidenShrine","Haiden Shrine");
        // Abbot
        npcDialogHeaderDictionary.Add("Abbot_01", "The Abbot");
        // Gossip monk #1
        npcDialogHeaderDictionary.Add("Haiden_Gossiping_Monk_01", "Corrupt Monk 1");
        // Gossip monk #2
        npcDialogHeaderDictionary.Add("Haiden_Gossiping_Monk_02", "Corrupt Monk 2");
        // eaves dropping game object ..
        npcDialogHeaderDictionary.Add("EavesDropSpot", "...");
        // the evidence ...
        npcDialogHeaderDictionary.Add("FinalEvidence", "...");

        npcDialogDictionary.Add("Temple_Guard1", "Hello there! Welcome to Oda Nobunaga's temple. Please pray and offer your ablutions at the Chōzuya"); // this triggers an objective ..z
        npcDialogDictionary.Add("Temple_Guard2", "Hello there! Welcome to Oda Nobunaga's temple. Only purified pilgrims and monks have access to the main temple"); // this triggers an objective ..
        npcDialogDictionary.Add("ByStander_01", "Praying... yet his facial expression suggests otherwise.");
        npcDialogDictionary.Add("AbolutionShrine_01", "Ablution offered ...");
        npcDialogDictionary.Add("FestivalAttendant_01", "Sorry, I can't talk now. The sacred dance (kagura) and music are being performed");
        npcDialogDictionary.Add("EmaGameItem", "You have earned the trust of the devout monks by inscribing a prayer here. ...");
        npcDialogDictionary.Add("HaidenMonk_01", "Only pilgrims who have demonstrated their devotion may pass");
        npcDialogDictionary.Add("HaidenMonk_02", "Only pilgrims who have demonstrated their devotion may pass ... hmmm but what am I doing so far away from the temple?");
        npcDialogDictionary.Add("KaguraDenGameObjectIteraction", "You watched the play and gave a token of your appreciation");
        npcDialogDictionary.Add("HaidenShrine", "You prayed at the shrine, earning the monks' trust and the opportunity to meet their Abbot");
        npcDialogDictionary.Add("Abbot_01", "The Abbot stared at you, as if peering into your soul, and bestowed his blessings upon your cause");
        npcDialogDictionary.Add("Haiden_Gossiping_Monk_01", "We are discussing private...'spritual' matters and it is none of your business. Go away");
        npcDialogDictionary.Add("Haiden_Gossiping_Monk_02", "We are discussing private...'spritual' matters and it is none of your business. Go away");
        npcDialogDictionary.Add("EavesDropSpot", "You overhear two monks revealing that Oda Nobunaga has been bribing some monks and members of the Shogunate through the Admin office.");
        npcDialogDictionary.Add("FinalEvidence", "You have snuck into Shamusho shrine - the Admin office , and discovered there are invoices, payments and correspondence between Oda Nobunaga and high ranking monks.");
    }

    public static GlobalInfo Instance
    {
        get
        {
            if (_instance == null) _instance = new GlobalInfo();
            return _instance;
        }
    }

    public string GetNpcDialogHeader(string npcName)
    {
        if (npcDialogHeaderDictionary.ContainsKey(npcName))
        {
            return npcDialogHeaderDictionary[npcName];
        }
        return string.Empty;
    }

    public string GetNpcDialog(string npcName)
    {
        if (npcDialogDictionary.ContainsKey(npcName))
        {
            return npcDialogDictionary[npcName];
        }
        return string.Empty;
    }
}