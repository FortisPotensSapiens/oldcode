import { useMemo } from 'react';

import { useGetConfig } from '~/api';

import { CONFIG_DOCUMENTS_MAPPING, CONFIG_DOCUMENTS_TRANSLATIONS } from '../DocumentsList/types';

const useDocumentsMenu = () => {
  const { data: config } = useGetConfig();

  return useMemo(
    () =>
      CONFIG_DOCUMENTS_MAPPING.map((item) => {
        return {
          id: item,
          title: CONFIG_DOCUMENTS_TRANSLATIONS[item],
          to: (config as any)?.[item as any],
          target: '_blank',
          external: true,
        };
      }),
    [config],
  );
};

export { useDocumentsMenu };
