import { useEffect, useState } from 'react';

import { useGetConfig } from '~/api';

import { DocumentsList } from './DocumentsList';
import { CONFIG_DOCUMENTS_MAPPING } from './types';

const DocumentsListContainer = () => {
  const { data: config } = useGetConfig();
  const [documents, updateDocuments] = useState<Record<string, string>>({});

  useEffect(() => {
    if (config) {
      updateDocuments(
        CONFIG_DOCUMENTS_MAPPING.reduce<Record<string, string>>((acc, value: string) => {
          acc[value] = (config as any)[value];

          return acc;
        }, {}),
      );
    }
  }, [config]);

  return <DocumentsList documents={documents} />;
};

export { DocumentsListContainer };
