import React from 'react';
import Giscus from "@giscus/react";
import { useColorMode } from '@docusaurus/theme-common';

export default function GiscusComponent() {
  const { colorMode } = useColorMode();

  return (
    <Giscus    
      repo="simpleidserver/DiveIt"
      repoId="R_kgDOLSEAaQ"
      category="General"
      categoryId="DIC_kwDOLSEAac4CduH8"  // E.g. id of "General"
      mapping="url"                        // Important! To map comments to URL
      term="Welcome to simpleidserver/DiveIt component!"
      strict="0"
      reactionsEnabled="1"
      emitMetadata="1"
      inputPosition="top"
      theme={colorMode}
      lang="en"
      loading="lazy"
      crossorigin="anonymous"
      async
    />
  );
}