import { History, PersonOutline } from '@mui/icons-material';
import { useMemo } from 'react';

import { useIsLogged } from '~/hooks';
import { getOrdersPath, getProfilePath } from '~/routing';

import { MenuItem } from './types';

type UseCommonMenu = () => MenuItem[];

const useCommonMenu: UseCommonMenu = () => {
  const isLogged = useIsLogged();

  return useMemo(
    () =>
      isLogged
        ? [
            { icon: PersonOutline, id: 'profile', title: 'Мой профиль', to: getProfilePath() },
            { icon: History, id: 'orders history', title: 'История заказов', to: getOrdersPath() },
          ]
        : [],
    [isLogged],
  );
};

export { useCommonMenu };
export type { UseCommonMenu };
