using System;
using Sirenix.Utilities;
using UnityEngine;

namespace Omega.Kulibin
{
    public sealed class User
    {
        private string _email = string.Empty;

        public readonly string Id;

        public event Action<string> OnEmailUpdated;

        public User(IFileSystemService fileSystem, Constants constants)
        {
            Id = fileSystem.ReadFile(constants.PathToUserId);

            if (Id.IsNullOrWhitespace())
            {
                Id = SystemInfo.deviceUniqueIdentifier;
                fileSystem.WriteToFile(constants.PathToUserId, Id);
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnEmailUpdated?.Invoke(value);
            }
        }
    }
}