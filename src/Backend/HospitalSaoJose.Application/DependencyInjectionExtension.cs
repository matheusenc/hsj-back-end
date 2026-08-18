using HospitalSaoJose.Application.Mappings;
using HospitalSaoJose.Application.UseCases.Category.DeleteById;
using HospitalSaoJose.Application.UseCases.Category.GetAll;
using HospitalSaoJose.Application.UseCases.Category.Register;
using HospitalSaoJose.Application.UseCases.Category.UpdateById;
using HospitalSaoJose.Application.UseCases.Document.DeleteById;
using HospitalSaoJose.Application.UseCases.Document.Download;
using HospitalSaoJose.Application.UseCases.Document.Filter;
using HospitalSaoJose.Application.UseCases.Document.GetById;
using HospitalSaoJose.Application.UseCases.Document.Register;
using HospitalSaoJose.Application.UseCases.Document.UpdateById;
using HospitalSaoJose.Application.UseCases.Login.WithEmailAndPassword;
using HospitalSaoJose.Application.UseCases.Profile.DeleteById;
using HospitalSaoJose.Application.UseCases.Profile.GetAll;
using HospitalSaoJose.Application.UseCases.Profile.GetById;
using HospitalSaoJose.Application.UseCases.Profile.Register;
using HospitalSaoJose.Application.UseCases.Profile.UpdateById;
using HospitalSaoJose.Application.UseCases.Role.DeleteById;
using HospitalSaoJose.Application.UseCases.Role.GetAll;
using HospitalSaoJose.Application.UseCases.Role.Register;
using HospitalSaoJose.Application.UseCases.Role.UpdateById;
using HospitalSaoJose.Application.UseCases.User.ChangePassword;
using HospitalSaoJose.Application.UseCases.User.Deactivate;
using HospitalSaoJose.Application.UseCases.User.Filter;
using HospitalSaoJose.Application.UseCases.User.Logged;
using HospitalSaoJose.Application.UseCases.User.Register;
using HospitalSaoJose.Application.UseCases.User.UpdateById;
using Microsoft.Extensions.DependencyInjection;

namespace HospitalSaoJose.Application;

public static class DependencyInjectionExtension
{
    extension(IServiceCollection services)
    {
        public void AddApplication()
        {
            MapsterConfiguration.Configure();

            services.AddUseCases();
        }

        private void AddUseCases()
        {
            services.AddScoped<ILoginWithEmailAndPasswordUseCase, LoginWithEmailAndPasswordUseCase>();

            services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
            services.AddScoped<IUpdateUserByIdUseCase, UpdateUserByIdUseCase>();
            services.AddScoped<IChangePasswordUseCase, ChangePasswordUseCase>();
            services.AddScoped<IFilterUsersUseCase, FilterUsersUseCase>();
            services.AddScoped<IGetLoggedUserUseCase, GetLoggedUserUseCase>();
            services.AddScoped<IDeactivateUserUseCase, DeactivateUserUseCase>();

            services.AddScoped<IRegisterProfileUseCase, RegisterProfileUseCase>();
            services.AddScoped<IUpdateProfileByIdUseCase, UpdateProfileByIdUseCase>();
            services.AddScoped<IDeleteProfileByIdUseCase, DeleteProfileByIdUseCase>();
            services.AddScoped<IGetAllProfilesUseCase, GetAllProfilesUseCase>();
            services.AddScoped<IGetProfileByIdUseCase, GetProfileByIdUseCase>();

            services.AddScoped<IRegisterRoleUseCase, RegisterRoleUseCase>();
            services.AddScoped<IUpdateRoleByIdUseCase, UpdateRoleByIdUseCase>();
            services.AddScoped<IDeleteRoleByIdUseCase, DeleteRoleByIdUseCase>();
            services.AddScoped<IGetAllRolesUseCase, GetAllRolesUseCase>();

            services.AddScoped<IRegisterCategoryUseCase, RegisterCategoryUseCase>();
            services.AddScoped<IUpdateCategoryByIdUseCase, UpdateCategoryByIdUseCase>();
            services.AddScoped<IDeleteCategoryByIdUseCase, DeleteCategoryByIdUseCase>();
            services.AddScoped<IGetAllCategoriesUseCase, GetAllCategoriesUseCase>();

            services.AddScoped<IRegisterDocumentUseCase, RegisterDocumentUseCase>();
            services.AddScoped<IUpdateDocumentByIdUseCase, UpdateDocumentByIdUseCase>();
            services.AddScoped<IDeleteDocumentByIdUseCase, DeleteDocumentByIdUseCase>();
            services.AddScoped<IFilterDocumentsUseCase, FilterDocumentsUseCase>();
            services.AddScoped<IGetDocumentByIdUseCase, GetDocumentByIdUseCase>();
            services.AddScoped<IDownloadDocumentUseCase, DownloadDocumentUseCase>();
        }
    }
}
