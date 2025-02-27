import {
  FormControl,
  FormControlLabel,
  FormLabel,
  Input,
  Radio,
  RadioGroup,
  Stack,
} from "@mui/material";
import React, { useEffect, useState } from "react";
import { useTranslation } from "react-i18next";
import { useNavigate } from "react-router-dom";
import logo from "../../assets/images/logo.png";
import { Select } from "../../components/widgets/Select";
import TextInput from "../../components/widgets/TextInput";
import { roleTypes } from "../../constants";
import resources from "../../resources/resources";
import useRoleService from "../../services/RoleService";
import useSalonService from "../../services/SalonService";
import useServiceEntityService from "../../services/ServiceEntityService";
import useAuthService from "../../services/UserService";
import "../../styles/pages/register.scss";
import VALIDATIONS from "../../validations";
import { useValidation } from "../../validations/useValidation";
import Validator from "../../validations/Validator";
import CreateSalon from "../Salon/CreateSalon";

const Register = () => {
  const [role, setRole] = React.useState("");
  const [salon, setSalon] = React.useState("");
  const [selectedServiceList, setSelectedServiceList] = React.useState([]);
  const [roleList, setRoleList] = React.useState([]);
  const [salonList, setSalonList] = React.useState([]);
  const [serviceList, setServiceList] = React.useState([]);
  const [isStylist, setIsStylist] = React.useState(false);
  const [isManager, setIsManager] = React.useState(false);
  const [salonExists, setSalonExists] = React.useState(true);
  const [salonForm, setSalonForm] = useState({});
  const [profilePicture, setProfilePicture] = useState("");
  const [verifiedPassword, setVerifiedPassword] = useState(undefined);

  const { t } = useTranslation();

  const navigate = useNavigate();

  const formValidator = new Validator()
    .forProperty("email")
    .check(VALIDATIONS.isRequired, "Missing")
    .forProperty("password")
    .check(VALIDATIONS.isRequired, "Message de 4 carac minim")
    .forProperty("firstName")
    .check(VALIDATIONS.isRequired, "Missing")
    // .forProperty("confirmPassword")
    // .check(VALIDATIONS.isRequired, "Missing")
    .forProperty("lastName")
    .forProperty("phoneNumber")
    .forProperty("latitude")
    .forProperty("longitude")
    .applyCheckOnlyOnSubmit();

  const {
    values: formData,
    errors: formErrors,
    onChangeInput,
    handleCheckFormErrors,
    applyErrorsFromApi,
  } = useValidation(formValidator);

  const { register } = useAuthService();

  const { getRoles } = useRoleService();

  const { getSalons } = useSalonService();

  const { getServices } = useServiceEntityService();

  const fetchRoles = async () => {
    const roles = await getRoles();
    setRoleList(roles);
  };

  const fetchSalons = async () => {
    const salons = await getSalons();
    setSalonList(salons);
  };

  const fetchServices = async () => {
    const services = await getServices();
    setServiceList(services);
  };

  const handleRadioButtonSalonExists = (event) => {
    setSalonExists(true);
  };

  const handleRadioButtonSalonDoesNotExist = (event) => {
    setSalonExists(false);
  };

  useEffect(() => {
    fetchRoles();
    fetchSalons();
    fetchServices();
  }, []);

  const handleSubmit = async () => {
    if (verifiedPassword != formData.password) {
      formErrors["confirmPassword"] = "trebuie sa coincida";
    }

    if (!handleCheckFormErrors()) {
      try {
        await register({
          registerForm: {
            email: formData.email,
            password: formData.password,
            roleId: role,
            firstName: formData.firstName,
            lastName: formData.lastName,
            profilePicture: profilePicture,
            phoneNumber: formData.phoneNumber,
            salonId: salon,
            services: selectedServiceList,
          },
          salonForm:
            isManager && !salonExists
              ? {
                  ...salonForm,
                  cityId: salonForm.city,
                  countyId: salonForm.county,
                }
              : null,
        }).then(() => navigate("/login"));
      } catch (err) {
        applyErrorsFromApi(err.message.errors);
      }
    }
  };

  const handleRoleSelect = (event) => {
    setRole(event.target.value);
    if (event.target.value === roleTypes.stylistId) {
      setIsStylist(true);
      setIsManager(false);
      setSalonForm({});
    }
    if (event.target.value === roleTypes.managerId) {
      setIsManager(true);
      setIsStylist(false);
    }
    if (
      event.target.value != roleTypes.managerId &&
      event.target.value != roleTypes.stylistId
    ) {
      setIsManager(false);
      setIsStylist(false);
      setSalonForm({});
    }
  };

  const onFileChange = (event) => {
    setProfilePicture(event.target.files[0]);
  };

  const handleSalonSelect = (event) => {
    setSalon(event.target.value);
  };

  const handleServicesSelect = (event) => {
    setSelectedServiceList([...event.target.value]);
  };

  return (
    <div className="register-page">
      <div className="register-form">
        <h1>{t(resources.title)}</h1>
        <Stack gap={3}>
          <TextInput
            error={formErrors["email"]}
            label={t(resources.register.emailLabel)}
            variant="standard"
            name="email"
            value={formData.email}
            onChange={onChangeInput}
          ></TextInput>
          <TextInput
            error={formErrors["password"]}
            label={t(resources.register.passwordLabel)}
            variant="standard"
            type="password"
            name="password"
            value={formData.password}
            onChange={onChangeInput}
          ></TextInput>
          <TextInput
            error={formErrors["firstName"]}
            label={t(resources.register.firstNameLabel)}
            variant="standard"
            type="firstName"
            name="firstName"
            value={formData.firstName}
            onChange={onChangeInput}
          ></TextInput>
          <TextInput
            label={t(resources.register.lastNameLabel)}
            variant="standard"
            type="lastName"
            name="lastName"
            value={formData.lastName}
            onChange={onChangeInput}
          ></TextInput>
          <Input
            onChange={onFileChange}
            placeholder="description"
            type="file"
            accept="image/*"
          ></Input>

          <TextInput
            error={formErrors["phoneNumber"]}
            label={t(resources.register.phoneNumber)}
            variant="standard"
            type="phoneNumber"
            name="phoneNumber"
            value={formData.phoneNumber}
            onChange={onChangeInput}
          ></TextInput>
          <Select
            options={roleList}
            value={role}
            label={t(resources.register.roleLabel)}
            onChange={handleRoleSelect}
          ></Select>

          {isManager && !isStylist && (
            <FormControl>
              <FormLabel id="demo-radio-buttons-group-label"></FormLabel>
              <RadioGroup
                aria-labelledby="demo-radio-buttons-group-label"
                defaultValue="Register for an already existing salon"
              >
                <FormControlLabel
                  value="Create new salon"
                  control={<Radio />}
                  label="Create new salon"
                  onClick={() => handleRadioButtonSalonDoesNotExist()}
                />
                <FormControlLabel
                  value="Register for an already existing salon"
                  control={<Radio />}
                  label="Register for an already existing salon"
                  onClick={() => handleRadioButtonSalonExists()}
                />
              </RadioGroup>
            </FormControl>
          )}

          {(isStylist || (isManager && salonExists)) && (
            <Select
              options={salonList}
              value={salon}
              label={t(resources.register.salonLabel)}
              onChange={handleSalonSelect}
            ></Select>
          )}
          {!isManager && isStylist && (
            <Select
              options={serviceList}
              value={selectedServiceList}
              label="services"
              multiple
              onChange={handleServicesSelect}
            ></Select>
          )}

          {isManager && !salonExists && (
            <CreateSalon
              salon={salonForm}
              setSalon={setSalonForm}
            ></CreateSalon>
          )}

          <button className="register-login-button" onClick={handleSubmit}>
            REGISTER
          </button>
        </Stack>
      </div>
      <div className="register-logo">
        <img
          src={logo}
          style={{
            objectFit: "cover",
            width: 350,
            height: 300,
            padding: 5,
          }}
        ></img>
      </div>
    </div>
  );
};

export default Register;
