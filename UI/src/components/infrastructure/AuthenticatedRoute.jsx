import userSession from "../../utils/userSession";
import { Navigate } from "react-router-dom";

function AuthenticatedRoute({ children }) {
  return userSession.isAuthenticated() ? children : <Navigate to="/login" />;
}
export default AuthenticatedRoute;
