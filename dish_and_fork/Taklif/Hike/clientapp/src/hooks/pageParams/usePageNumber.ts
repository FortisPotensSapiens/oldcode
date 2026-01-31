import { useConfig } from '~/contexts';

import { UseParam, useParam } from './useParam';

type UsePageNumber = (initial?: number) => ReturnType<UseParam>;

const usePageNumber: UsePageNumber = (initial = 1) => {
  const { pages } = useConfig();

  return useParam(pages.pageNumberParamName, initial);
};

export type { UsePageNumber };
export { usePageNumber };
