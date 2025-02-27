import { Typography } from "@mui/material";
import { useTranslation } from "react-i18next";
import { useLocation } from "react-router-dom";
import resources from "../resources/resources";
import Paths from "../statics/Paths";

export default function NotFound() {
  let location = useLocation();
  let { t } = useTranslation();

  return (
    <div>
      <h3>
        {t(resources.notFound.title0)}
        <code>
          {import.meta.env.VITE_UI_URL}
          {location.pathname}
        </code>
        {t(resources.notFound.title1)}
      </h3>
      {t(resources.notFound.backLink)}{" "}
      <a href={Paths.home}>
        <Typography color={"black"}>{t(resources.notFound.backLinkText)}</Typography>
      </a>
    </div>
  );
}
