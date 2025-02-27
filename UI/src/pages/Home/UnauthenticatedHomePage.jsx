import { useNavigate } from "react-router-dom";
import logo from "../../assets/images/beautylink-high-resolution-logo-transparent.png";

const UnauthenticatedHomePage = () => {
  const navigate = useNavigate();

  const handleLoginClick = () => {
    navigate("/login");
  };

  const handleRegisterClick = () => {
    navigate("/register");
  };
  return (
    <div>
      <img
        src={logo}
        style={{
          width: 900,
          display: "block",
          marginLeft: "auto",
          marginRight: "auto",
          marginTop: 300,
          marginBottom: 100,
        }}
      ></img>
      <div className="buttons-unauth">
        <button
          className="register-login-button-unauth"
          onClick={handleLoginClick}
        >
          LOGIN
        </button>
        <button
          className="register-login-button-unauth"
          onClick={handleRegisterClick}
        >
          REGISTER
        </button>
      </div>
    </div>
  );
};

export default UnauthenticatedHomePage;
