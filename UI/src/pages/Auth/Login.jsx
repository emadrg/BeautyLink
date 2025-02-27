import { Alert, FormControl, Input, InputLabel, Stack } from "@mui/material";
import { useEffect } from "react";
import { useTranslation } from "react-i18next";
import { useNavigate } from "react-router-dom";
import { useAccount } from "../../contexts/AccountContext";
import resources from "../../resources/resources";
import useAuthService from "../../services/UserService";
import userSession from "../../utils/userSession";
import VALIDATIONS from "../../validations";
import { useValidation } from "../../validations/useValidation";
import Validator from "../../validations/Validator";

const LoginPage = () => {
  const { t } = useTranslation();
  const { login } = useAuthService();
  const { setIsAuth } = useAccount();
  const navigate = useNavigate();

  const {
    values: loginDetails,
    errors,
    onChangeInput,
    handleCheckFormErrors,
    applyErrorsFromApi,
  } = useValidation(
    new Validator()
      .forProperty("email")
      .check(VALIDATIONS.isRequired, "Error is required")
      .forProperty("password")
      .check(VALIDATIONS.isRequired, "Error is required")
      .check(VALIDATIONS.minLength(3), "Min length 3")
  );

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (!handleCheckFormErrors()) {
      try {
        await login(loginDetails);
        setIsAuth(true);
        navigate("/");
      } catch (err) {
        applyErrorsFromApi(err.message.errors);
      }
    }
  };

  useEffect(() => {
    userSession.clearAuthSession();
  }, []);

  return (
    <div>
      <form onSubmit={handleSubmit}>
        <h1> {t(resources.login.title)}</h1>
        <Stack gap={3}>
          <FormControl>
            <InputLabel>{t(resources.login.emailLabel)}</InputLabel>
            <Input
              name="email"
              value={loginDetails.email}
              onChange={onChangeInput}
              error={errors.email}
            ></Input>
          </FormControl>
          <FormControl>
            <InputLabel>{t(resources.login.passwordLabel)}</InputLabel>
            <Input
              input
              type="password"
              name="password"
              value={loginDetails.password}
              onChange={onChangeInput}
              error={errors.password}
            />
            {errors["password"] && (
              <Alert severity="error">{errors["password"]}</Alert>
            )}
          </FormControl>
          <button className="register-login-button" type="submit">
            LOGIN
          </button>
        </Stack>
      </form>
    </div>
  );
};

export default LoginPage;
