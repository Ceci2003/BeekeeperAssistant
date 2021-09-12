namespace BeekeeperAssistant.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Data;
    using BeekeeperAssistant.Web.ViewModels.Apiaries;
    using BeekeeperAssistant.Web.ViewModels.Beehives;
    using BeekeeperAssistant.Web.ViewModels.Treatments;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class TreatmentController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ITreatmentService treatmentService;
        private readonly IApiaryService apiaryService;
        private readonly IBeehiveService beehiveService;

        public TreatmentController(
            UserManager<ApplicationUser> userManager,
            ITreatmentService treatmentService,
            IApiaryService apiaryService,
            IBeehiveService beehiveService)
        {
            this.userManager = userManager;
            this.treatmentService = treatmentService;
            this.apiaryService = apiaryService;
            this.beehiveService = beehiveService;
        }

        public async Task<IActionResult> Create(int? id)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);

            var inputModel = new CreateTreatmentInputModel()
            {
                DateOfTreatment = DateTime.UtcNow.Date,
            };

            if (id == null)
            {
                inputModel.Apiaries = this.apiaryService.GetUserApiariesAsKeyValuePairs(currentUser.Id);
            }
            else
            {
                inputModel.ApiaryId = this.apiaryService.GetApiaryIdByBeehiveId(id.Value);
            }

            return this.View(inputModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(int? id, CreateTreatmentInputModel inputModel)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);

            if (id == null && !inputModel.AllBeehives)
            {
                if (inputModel.BeehiveNumbersSpaceSeparated != null)
                {
                    try
                    {
                        var beehives = this.beehiveService.GetApiaryBeehivesById<BeehiveViewModel>(inputModel.ApiaryId).Select(b => b.Number).ToList();
                        var selectedNumbers = inputModel.BeehiveNumbersSpaceSeparated.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(n => Convert.ToInt32(n)).ToList();

                        foreach (var number in selectedNumbers)
                        {
                            if (!beehives.Contains(number))
                            {
                                this.ModelState.AddModelError(string.Empty, $"Не съществува кошер с номер {number} в пчелина!");
                            }
                        }
                    }
                    catch (Exception)
                    {
                        this.ModelState.AddModelError(string.Empty, "Номерата на кошерите не са въведени правилно!");
                    }
                }
                else
                {
                    this.ModelState.AddModelError(string.Empty, "Изберете кошери!");
                }
            }

            if (!this.ModelState.IsValid)
            {
                inputModel.DateOfTreatment = DateTime.UtcNow.Date;
                if (id == null)
                {
                    inputModel.Apiaries = this.apiaryService.GetUserApiariesAsKeyValuePairs(currentUser.Id);
                }

                return this.View(inputModel);
            }

            var apiaryNumber = this.apiaryService.GetApiaryById<ApiaryViewModel>(inputModel.ApiaryId).Number;

            if (id == null)
            {
                var apiaryBeehives = this.beehiveService.GetApiaryBeehivesById<BeehiveViewModel>(inputModel.ApiaryId).ToList();
                if (inputModel.AllBeehives)
                {
                    var beehivesIds = apiaryBeehives.Select(b => b.Id).ToList();
                    await this.treatmentService.CreateTreatment(
                    currentUser.Id,
                    inputModel.DateOfTreatment,
                    inputModel.Name,
                    inputModel.Note,
                    inputModel.Disease,
                    inputModel.Medication,
                    inputModel.InputAs,
                    inputModel.Quantity,
                    inputModel.Dose,
                    beehivesIds);
                }
                else
                {
                    var selectedIds = new List<int>();
                    var selectedBeehiveNumbers = inputModel.BeehiveNumbersSpaceSeparated.Split(' ').Select(n => Convert.ToInt32(n)).ToList();
                    foreach (var number in selectedBeehiveNumbers)
                    {
                        var beehive = apiaryBeehives.Where(b => b.Number == number).FirstOrDefault();
                        if (beehive != null)
                        {
                            selectedIds.Add(beehive.Id);
                        }
                    }

                    await this.treatmentService.CreateTreatment(
                            currentUser.Id,
                            inputModel.DateOfTreatment,
                            inputModel.Name,
                            inputModel.Note,
                            inputModel.Disease,
                            inputModel.Medication,
                            inputModel.InputAs,
                            inputModel.Quantity,
                            inputModel.Dose,
                            selectedIds);
                }

                return this.RedirectToAction("Index", "Home");
            }
            else
            {
                await this.treatmentService.CreateTreatment(
                currentUser.Id,
                inputModel.DateOfTreatment,
                inputModel.Name,
                inputModel.Note,
                inputModel.Disease,
                inputModel.Medication,
                inputModel.InputAs,
                inputModel.Quantity,
                inputModel.Dose,
                new List<int> { id.Value });

                return this.RedirectToAction("ById", "Beehive", new { beehiveId = id.Value, tabPage = "Treatments" });
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            var inputModel = this.treatmentService.GetTreatmentById<EditTreatmentInputModel>(id);

            return this.View(inputModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditTreatmentInputModel inputModel)
        {
            // ToDo: make quantity string
            // var quantity = Convert.ToDouble(inputModel.Quantity);

            await this.treatmentService.EditTreatment(
                id,
                inputModel.BeehiveId.Value,
                inputModel.DateOfTreatment,
                inputModel.Name,
                inputModel.Note,
                inputModel.Disease,
                inputModel.Medication,
                inputModel.InputAs,
                inputModel.Quantity,
                inputModel.Dose);

            return this.RedirectToAction("ById", "Beehive", new { beehiveId = inputModel.BeehiveId.Value, tabPage = "Treatments" });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id, int? beehiveId)
        {
            var treatment = this.treatmentService.GetTreatmentById<TreatmentDataViewModel>(id);
            var currentUser = await this.userManager.GetUserAsync(this.User);

            if (treatment.CreatorId != currentUser.Id)
            {
                return this.BadRequest();
            }

            await this.treatmentService.DeleteTreatmentAsync(id);

            return this.RedirectToAction("ById", "Beehive", new { beehiveId = beehiveId, tabPage = "Treatments" });
        }
    }
}
