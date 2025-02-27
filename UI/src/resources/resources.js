import enTranslation from "./en.json";

const convertLanguageJsonToObject = (json, current) => {
  const result = {};

  Object.keys(json).forEach((key) => {
    const currentLookupKey = current ? `${current}.${key}` : key;

    if (typeof json[key] === "object") {
      result[key] = convertLanguageJsonToObject(json[key], currentLookupKey);
    } else {
      result[key] = currentLookupKey;
    }
  });

  return result;
};

let resources = enTranslation;

resources = convertLanguageJsonToObject(enTranslation);

export default resources;
