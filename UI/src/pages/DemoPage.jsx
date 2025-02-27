import { useValidation } from "../validations/useValidation";
import {
  Alert,
  Button,
  Checkbox,
  FormControl,
  FormLabel,
  MenuItem,
  Select,
  Stack,
  TextField,
  Typography,
} from "@mui/material";
import VALIDATIONS from "../validations/index";
import Validator from "../validations/Validator";
import useAuthService from "../services/UserService";

import { useTranslation } from "react-i18next";
import resources from "../resources/resources";
import { useEffect } from "react";

export default function DemoPage() {
  const { t } = useTranslation();

  const { register, fetchDetails } = useAuthService();

  const formValidator = new Validator()
    .forProperty("email")
    .check(VALIDATIONS.isRequired, "Missing")
    .check(VALIDATIONS.minLength(6), "Mesah de 6 carac")
    .forProperty("password")
    .check(VALIDATIONS.maxLength(6), "Mesaj de 6 caractere maxim")
    .check((val) => val.length > 4, "Message de 4 carac minim")
    .forProperty("email.name", [10])
    .check(VALIDATIONS.isRequired, "Missing")
    .forProperty("email.name.isAdmin", true)
    .check(VALIDATIONS.isRequired, "Needs checked");

  const {
    values: formData,
    setValues: setFormData,
    errors: formErrors,
    onChangeInput,
    handleCheckFormErrors,
    applyErrorsFromApi,
  } = useValidation(formValidator);

  useEffect(() => {
    const fetchUserDetails = async () => {
      return await fetchDetails();
    };

    fetchUserDetails().then((res) => {
      setFormData((prevState) => ({
        ...prevState,
        email: res.email,
      }));
    });
  }, []);

  const handleSubmit = async () => {
    if (!handleCheckFormErrors()) {
      try {
        await register(formData).then(() => navigate("/login"));
      } catch (err) {
        applyErrorsFromApi(err.message.errors);
      }
    }
  };

  return (
    <div>
      <Typography variant="h4" textAlign="left" my={4}>
        Demo Page - Validations
      </Typography>
      <Stack gap={3}>
        <FormControl>
          <TextField
            error={formErrors["email"]}
            label={t(resources.register.emailLabel)}
            variant="standard"
            name="email"
            value={formData.email}
            onChange={onChangeInput}
          />
          {formErrors["email"] && (
            <Alert severity="error">{formErrors["email"]}</Alert>
          )}
        </FormControl>
        <FormControl>
          <TextField
            error={formErrors["password"]}
            label={t(resources.register.passwordLabel)}
            variant="standard"
            type="password"
            name="password"
            value={formData.password}
            onChange={onChangeInput}
          />
          {formErrors["password"] && (
            <Alert severity="error">{formErrors["password"]}</Alert>
          )}
        </FormControl>

        <FormControl>
          <Select
            name="name"
            multiple
            value={formData.name}
            onChange={onChangeInput}
          >
            <MenuItem value={10}>10</MenuItem>
            <MenuItem value={20}>20</MenuItem>
            <MenuItem value={30}>30</MenuItem>
          </Select>
          {formErrors["name"] && (
            <Alert severity="error">{formErrors["name"]}</Alert>
          )}
        </FormControl>

        <FormControl>
          <FormLabel>Checkbox Demo</FormLabel>

          <Checkbox
            name="isAdmin"
            checked={formData["isAdmin"]}
            onChange={onChangeInput}
          />
          {formErrors["isAdmin"] && (
            <Alert severity="error">{formErrors["isAdmin"]}</Alert>
          )}
        </FormControl>

        <Button variant="outlined" onClick={handleSubmit}>
          {t(resources.register.registerButton)}
        </Button>
      </Stack>
    </div>
  );
}
