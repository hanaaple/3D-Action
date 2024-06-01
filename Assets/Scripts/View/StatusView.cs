using System.ComponentModel;
using Data;
using TMPro;
using UnityEngine;

namespace View
{
    public class StatusView : MonoBehaviour
    {
        // Display Status Data
        
        public TMP_Text level;
        public TMP_Text experiencePoint;
        public TMP_Text levelUpExp;
        
        
        public TMP_Text constitution;
        public TMP_Text spirit;
        public TMP_Text strength;
        public TMP_Text stamina;
        
        private void Start()
        {
            var statusViewModel = DataManager.instance.statusViewModel;
            statusViewModel.PropertyChanged += UpdateStatusView;
        }
        
        private void UpdateStatusView(object sender, PropertyChangedEventArgs e)
        {
            var statusViewModel = DataManager.instance.statusViewModel;
            
            level.text = statusViewModel.Level.ToString();
            experiencePoint.text = statusViewModel.ExperiencePoint.ToString();

            if (statusViewModel.IsLevelUpPossible())
            {
                levelUpExp.text = statusViewModel.RequiredExperiencePoint.ToString();
            }
            else
            {
                levelUpExp.text = "";
            }
            
            constitution.text = statusViewModel.Constitution.ToString();
            spirit.text = statusViewModel.Spirit.ToString();
            strength.text = statusViewModel.Strength.ToString();
            stamina.text = statusViewModel.Stamina.ToString();
        }
    }
}