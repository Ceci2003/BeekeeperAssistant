namespace BeekeeperAssistant.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public enum BeehiveSystem
    {
        Лангстрот = 1,
        [Display(Name = "Лангстрот-Рут")]
        ЛангстротРут = 2,
        Фарар = 3,
        [Display(Name = "Дадан-Блат")]
        ДаданБлат = 4,
        [Display(Name = "Многокорпусен Кошер")]
        МногокорпусенКошер = 5,
        [Display(Name = "Тип Павилион")]
        ТипПавилион = 6,
        Лежак = 7,
        Тръвна = 8,
        Полистиролни = 8,
        [Display(Name = "Алпийският кошер Роже Делон")]
        АлпийскиятКошерРожеДелон = 10,
        [Display(Name = "С въртящи рамки")]
        СВъртящиРамки = 11,
        [Display(Name = "За отглеждане на майки")]
        ЗаОтглежданеНаМайки = 12,
    }
}
