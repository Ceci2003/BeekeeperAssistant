namespace BeekeeperAssistant.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public enum BeehiveSystem
    {
        [Display(Name = "Лангстрот")]
        Langstroth = 1,
        [Display(Name = "Лангстрот-Рут")]
        LangstrothRuth = 2,
        [Display(Name = "Фарар")]
        Farrar = 3,
        [Display(Name = "Дадан-Блат")]
        DadanBlatt = 4,
        [Display(Name = "Многокорпусен Кошер")]
        MultiHullKosher = 5,
        [Display(Name = "Тип Павилион")]
        PavilionType = 6,
        [Display(Name = "Лежак")]
        Lying = 7,
        [Display(Name = "Тръвна")]
        Travna = 8,
        [Display(Name = "Полистиролни")]
        Polystyrene = 8,
        [Display(Name = "Алпийският кошер Роже Делон")]
        AlpineBeehiveRogerDelon = 10,
        [Display(Name = "С въртящи рамки")]
        WithRotatingFrames = 11,
        [Display(Name = "За отглеждане на майки")]
        ForRaisingMothers = 12,
    }
}
