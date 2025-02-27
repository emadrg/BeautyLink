import { roleTypes } from "../../constants";
import userSession from "../../utils/userSession";
import AdminHomePage from "./AdminHomePage";
import ClientHomePage from "./ClientHomePage";
import ManagerHomePage from "./ManagerHomePage";
import StylistHomePage from "./StylistHomePage";
import UnauthenticatedHomePage from "./UnauthenticatedHomePage";

export default function Home() {
  const user = userSession.user();

  if (!user) {
    return <UnauthenticatedHomePage />;
  }

  if (user.roleId === roleTypes.clientId) {
    return <ClientHomePage />;
  }

  if (user.roleId === roleTypes.stylistId) {
    return <StylistHomePage />;
  }

  if (user.roleId === roleTypes.managerId) {
    return <ManagerHomePage />;
  }

  if (user.roleId === roleTypes.adminId) {
    return <AdminHomePage />;
  }

  return <></>;
}
