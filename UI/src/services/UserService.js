import { roleTypes } from "../constants";
import ApiPaths from "../statics/ApiPaths";
import { jsonToFormData } from "../utils";
import useApi from "../utils/apiUtils";
import userSession from "../utils/userSession";

const useAuthService = () => {
  const api = useApi();

  const login = async ({ email, password }) => {
    const authResponse = await api.post(ApiPaths.Authenticate, {
      id: email,
      key: password,
      AppId: import.meta.env.VITE_CLIENT_ID,
      AppKey: import.meta.env.VITE_CLIENT_SECRET,
    });

    userSession.saveAuthSession(authResponse.jwt, authResponse.userDetails);
  };

  const register = async ({ registerForm, salonForm }) => {
    let registerPath = "";
    let registerObject = {};
    switch (registerForm.roleId) {
      case roleTypes.clientId:
        registerPath = ApiPaths.RegisterUser;
        registerObject = registerForm;
        break;
      case roleTypes.stylistId:
        registerPath = ApiPaths.RegisterStylist;
        registerObject = registerForm;
        break;
      case roleTypes.managerId:
        registerPath = ApiPaths.RegisterManager;
        registerObject = {
          manager: registerForm,
          salon: salonForm,
        };
        break;
      default:
        registerPath = ApiPaths.RegisterUser;
        break;
    }

    if (registerForm.roleId === roleTypes.managerId) {
      let managerRegisterObject = {};
      Object.entries(registerObject.manager).forEach(
        ([key, value]) => (managerRegisterObject[`Manager.${key}`] = value)
      );

      let salonRegisterObject = {};
      if (registerObject.salon != null) {
        Object.entries(registerObject.salon).forEach(
          ([key, value]) => (salonRegisterObject[`Salon.${key}`] = value)
        );
      }

      let formDataJson = {};
      Object.entries(managerRegisterObject).forEach(
        ([key, value]) => (formDataJson[key] = value)
      );
      Object.entries(salonRegisterObject).forEach(
        ([key, value]) => (formDataJson[key] = value)
      );

      registerObject = formDataJson;
    }

    await api.post(
      registerPath,
      jsonToFormData(registerObject),
      "multipart/form-data"
    );
  };

  const fetchDetails = async () => {
    const user = userSession.user();

    if (user && user.id) {
      const userDetails = await api.get(ApiPaths.GetUserById(user.id));
      return userDetails;
    }

    return {};
  };

  const getProfilePictureName = async () => {
    const user = userSession.user();
    let picture = "";
    if (user.roleId === roleTypes.stylistId) {
      picture = await api.get(ApiPaths.GetProfilePictureName(user.userId));
    } else {
      picture = await api.get(ApiPaths.GetProfilePictureName(user.id));
    }

    return picture;
  };

  return {
    login,
    register,
    fetchDetails,
    getProfilePictureName,
  };
};

export default useAuthService;
