using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Quest
{
    public ImageIdEnum questIconImage;
    public string description;
    public ImageIdEnum rewardIconImage;
    public int rewardAmount;
    public int progressSize;
    public int progressValue;

    public Quest (ImageIdEnum questIconImage, string description, ImageIdEnum rewardIconImage, int rewardAmount, int progressSize, int progressValue = 0)
    {
        this.questIconImage = questIconImage;
        this.description = description;
        this.rewardIconImage = rewardIconImage;
        this.rewardAmount = rewardAmount;
        this.progressSize = progressSize;
        this.progressValue = progressValue;
    }
}
