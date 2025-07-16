using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Source.TiltBoard.Board
{
    public class SpawnPointGroupObject : MonoBehaviour
    {
        private List<Transform> _spawnPoints = null;

        void Awake()
        {
            _spawnPoints = GetComponentsInChildren<Transform>().ToList();

            // remove the first item, because that would be the self transform 
            _spawnPoints.RemoveAt(0);
        }

        /// <summary>
        /// Returns a list of spawn points picked from center outwards for uniform distribution.
        /// </summary>
        /// <param name="requiredCount">Number of spawn points needed.</param>
        public List<Transform> GetBalancedSpawnPoints(int requiredCount)
        {
            List<Transform> result = new List<Transform>();
            int totalPoints = _spawnPoints.Count;

            if (requiredCount >= totalPoints)
            {
                result.AddRange(_spawnPoints);
                return result;
            }

            int midIndex = totalPoints / 2;
            result.Add(_spawnPoints[midIndex]); // Always add center first

            int left = midIndex - 1;
            int right = midIndex + 1;

            while (result.Count < requiredCount)
            {
                // Alternate adding right and left to maintain symmetry
                if (right < totalPoints)
                {
                    result.Add(_spawnPoints[right]);
                    right++;
                    if (result.Count == requiredCount) break;
                }

                if (left >= 0)
                {
                    result.Add(_spawnPoints[left]);
                    left--;
                    if (result.Count == requiredCount) break;
                }
            }

            // Sort the final result list by their original index to maintain logical order
            result = result.OrderBy(t => _spawnPoints.IndexOf(t)).ToList();

            return result;
        }
    }
}
