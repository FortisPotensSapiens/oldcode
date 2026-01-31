import { useConfig } from '~/contexts';

import { UseParam, useParam } from './useParam';

type UsePageSize = (initial?: number) => ReturnType<UseParam>;

const usePageSize: UsePageSize = (initial) => {
  const { pages } = useConfig();

  return useParam(pages.pageSizeParamName, initial ?? pages.pageSize);
};

export { usePageSize };
export type { UsePageSize };
