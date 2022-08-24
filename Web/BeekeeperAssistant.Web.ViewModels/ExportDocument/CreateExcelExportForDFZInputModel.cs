namespace BeekeeperAssistant.Web.ViewModels.ExportDocument
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class CreateExcelExportForDFZInputModel : IValidatableObject
    {
        [Required(ErrorMessage = "Полето 'Имена' е задължително!")]
        [Display(Name = "Три имена")]
        public string UserNames { get; set; }

        [Required(ErrorMessage = "Полето 'ЕГН/ЕИК' е задължително!")]
        [Display(Name = "ЕГН/ЕИК")]
        public string EgnEik { get; set; }

        [Required(ErrorMessage = "Полето 'гр./с.' е задължително!")]
        [Display(Name = "гр./с.")]
        public string UserCity { get; set; }

        [Required(ErrorMessage = "Полето 'Община' е задължително!")]
        [Display(Name = "Община")]
        public string UserMunicipality { get; set; }

        [Required(ErrorMessage = "Полето 'Улица' е задължително!")]
        [Display(Name = "Улица")]
        public string UserStreet { get; set; }

        [Required(ErrorMessage = "Полето 'Номер' е задължително!")]
        [Display(Name = "Номер")]
        public int UserStreetNumber { get; set; }

        [Display(Name = "Блок")]
        public string UserApartmentBuilding { get; set; }

        [Display(Name = "Апартамент")]
        public string UserApartment { get; set; }

        [Display(Name = "Етаж")]
        public int UserFloor { get; set; }

        [Required(ErrorMessage = "Полето 'Телефон' е задължително!")]
        [Display(Name = "Телефон")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Полето '' е задължително!")]
        [Display(Name = "В качеството си на")]
        public string UserTypeAsFirst { get; set; }

        [Display(Name = "юридическо лице/ЕТ")]
        public string NameOfET { get; set; }

        [Required(ErrorMessage = "Полето 'В качеството си на' е задължително!")]
        [Display(Name = "В качеството си на")]
        public string UserTypeAsSecond { get; set; }

        [Required(ErrorMessage = "Полето 'Пчелин' е задължително!")]
        [Display(Name = "Пчелин")]
        public int ApiaryId { get; set; }

        public IEnumerable<KeyValuePair<int, string>> AllApiaries { get; set; }

        [Required(ErrorMessage = "Полето 'гр./с.' е задължително!")]
        [Display(Name = "гр./с.")]
        public string ApiaryCity { get; set; }

        [Required(ErrorMessage = "Полето 'Община' е задължително!")]
        [Display(Name = "Община")]
        public string ApiaryMunicipality { get; set; }

        [Required(ErrorMessage = "Полето 'Област' е задължително!")]
        [Display(Name = "Област")]
        public string ApiaryState { get; set; }

        [Required(ErrorMessage = "Полето '' е задължително!")]
        [Display(Name = "Име на ветеринарен лекар")]
        public string VetNames { get; set; }

        [Required(ErrorMessage = "Полето 'Дата на подаване' е задължително!")]
        [Display(Name = "Дата на подаване")]
        public DateTime? SubmissionDate { get; set; }

        [Required(ErrorMessage = "Полето 'Дата на описа' е задължително!")]
        [Display(Name = "Дата на описа")]
        public DateTime? InventoryDate { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var errorList = new List<ValidationResult>();

            if (this.UserApartmentBuilding != null)
            {
                if (string.IsNullOrWhiteSpace(this.UserApartment))
                {
                    errorList.Add(new ValidationResult("Въведете номер на апартамент!"));
                }

                if (this.UserFloor <= 0)
                {
                    errorList.Add(new ValidationResult("Въведете номер на етаж!"));
                }
            }

            return errorList;
        }
    }
}
