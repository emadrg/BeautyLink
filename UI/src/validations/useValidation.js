import { useState } from "react";

export const useValidation = (validator) => {
  const formData = validator.getConfiguration();
  const checkOnlyOnSubmit = validator.getCheckOnlyOnSubmit();

  const [values, setValues] = useState(
    Object.keys(formData).reduce((acc, el) => {
      acc[el] = formData[el].value;
      return acc;
    }, {})
  );
  const [errors, setErrors] = useState(
    Object.keys(formData).reduce((acc, el) => {
      acc[el] = "";
      return acc;
    }, {})
  );

  const onChangeInput = (ev) => {
    const { name, value, checked } = ev.target;
    const inputValue = typeof values[name] === "boolean" ? checked : value;
    setErrors({ ...errors, [name]: "" });

    setValues({
      ...values,
      [name]: inputValue,
    });

    if (checkOnlyOnSubmit) {
      return;
    }

    setErrors({
      ...errors,
      [name]: formData[name].validations.find(
        (validation) => !validation.check(inputValue)
      ).errorMessage,
    });
  };

  const handleCheckFormErrors = () => {
    let errors = {};
    Object.keys(values).map((key) => {
      const validation = formData[key].validations.find(
        (validation) => !validation.check(values[key])
      );
      errors[key] = validation ? validation.errorMessage : "";
    });
    setErrors({ ...errors });

    return (
      Object.keys(errors).filter(
        (key) => typeof errors[key] === "string" && errors[key].trim() !== ""
      ).length > 0
    );
  };

  const applyErrorsFromApi = (errors) => {
    let errorsToAppend = {};
    Object.keys(errors).map((key) => {
      errorsToAppend[key] = errors[key][0];
    });

    setErrors({ ...errors, ...errorsToAppend });
  };

  return {
    values,
    setValues,
    errors,
    onChangeInput,
    handleCheckFormErrors,
    applyErrorsFromApi,
  };
};
