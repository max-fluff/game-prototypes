using System;
using System.Collections.Generic;
using UnityEngine;

namespace MaxFluff.Prototypes
{
    [CreateAssetMenu(fileName = "PuzzleContactsList", menuName = "Config/Puzzle/ContactsList", order = 0)]
    public class ContactsList : ScriptableObject
    {
        public List<Contact> Contacts;
    }

    [Serializable]
    public struct Contact
    {
        public string Name;
        public string PhoneNumber;
        public string InitReply;
        public string InitNoDataReply;
        public List<PersonReaction> PersonReactions;
    }

    [Serializable]
    public struct PersonReaction
    {
        public string PhoneNumberToReact;
        public List<Reaction> Reactions;
    }

    [Serializable]
    public struct Reaction
    {
        public PersonData DataReaction;
        public string Reply;
    }
}