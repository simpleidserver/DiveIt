import React from 'react';
import { useLocation } from "@docusaurus/router";
import Docsly from "@docsly/react";
import "@docsly/react/styles.css";

export default function GiscusComponent() {
  const { pathname } = useLocation();
  return (
    <Docsly publicId="pk_BpvG0voKqRnlS9uTDlK77radoYWdawKXuuuhKTbrzptXXoRp" pathname={pathname} />
  );
}