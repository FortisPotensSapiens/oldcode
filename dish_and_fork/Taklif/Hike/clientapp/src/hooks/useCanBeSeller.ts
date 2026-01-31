import { useMemo } from 'react';

import { useConfig } from '~/contexts';
import { useMyUserProfile } from '~/hooks';

type UseCanBeSeller = (nullOnLoading?: boolean) => boolean | null;

const useCanBeSeller: UseCanBeSeller = (nullOnLoading = false) => {
  const { roles } = useConfig();
  const { data, isLoading } = useMyUserProfile();

  return useMemo(() => {
    if (nullOnLoading && isLoading) {
      return null;
    }

    return !isLoading && !data?.roles?.includes(roles.seller);
  }, [data?.roles, roles.seller, isLoading, nullOnLoading]);
};

export { useCanBeSeller };
export type { UseCanBeSeller };
