const minLength = (length) => (value) => value.length >= length;
const maxLength = (length) => (value) => value.length <= length;
const betweenLength = (length) => (value) =>
  minLength(value, length) && maxLength(value, length);
const isNumeric = (value) => value.match(/^[0-9]+$/i);
const isLowercase = (value) => value.match(/^[a-z]+$/i);
const isUppercase = (value) => value.match(/^[A-Z]+$/i);
const isAlphanumeric = (value) =>
  isNumeric(value) && isLowercase(value) && isUppercase(value);
const isEmail = (value) => value.match(/\S+@\S+\.\S+/);

const noValidation = () => true;

const isRequired = (value) =>
  value !== undefined &&
  value !== null &&
  ((typeof value === "string" && value !== "" && value.trim() !== "") ||
    (typeof value === "object" && value.length > 0) ||
    (typeof value === "boolean" && value === true));

const VALIDATIONS = {
  minLength,
  maxLength,
  betweenLength,
  isNumeric,
  isLowercase,
  isUppercase,
  isAlphanumeric,
  isEmail, 
  isRequired,
  noValidation
};

export default VALIDATIONS;
