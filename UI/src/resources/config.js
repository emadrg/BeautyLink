import i18n from "i18next";
import { initReactI18next } from "react-i18next";
import enTranslation from "./en.json";
import roTranslation from "./ro.json";

const resources = {
  ro: {
    translation: roTranslation,
  },
  en: {
    translation: enTranslation,
  },
};

i18n.use(initReactI18next).init({
  compatibilityJSON: "v3",
  lng: "en",
  interpolation: {
    escapeValue: false,
  },
  resources,
});
