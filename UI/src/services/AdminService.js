import ApiPaths from "../statics/ApiPaths";
import useApi from "../utils/apiUtils";

const useAdminService = () => {
  const api = useApi();

  const getAllUsers = async (roleId, skip, take) => {
    const users = await api.get(ApiPaths.GetAllUsers(roleId, skip, take));
    return users;
  };

  const updateUser = async (userObject) => {
    return await api.post(ApiPaths.UpdateUser, userObject);
  };

  return {
    getAllUsers,
    updateUser,
  };
};

export default useAdminService;
