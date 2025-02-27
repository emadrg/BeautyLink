import ApiPaths from "../statics/ApiPaths";
import useApi from "../utils/apiUtils";

const useRoleService = () => {
  const api = useApi();

  const getRoles = async () => {
    const roles = await api.get(ApiPaths.GetRoles);
    return roles;
  };

  return {
    getRoles,
  };
};

export default useRoleService;
