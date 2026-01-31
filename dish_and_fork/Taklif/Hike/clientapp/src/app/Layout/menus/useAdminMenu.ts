import { ListAlt, PeopleOutline, PlaylistAddCheck } from '@mui/icons-material';
import { useMemo } from 'react';

import { useConfig } from '~/contexts';
import { useMyUserProfile } from '~/hooks';
import {
  getAdminCollectionsPath,
  getAdminGlobalPath,
  getAdminProductsPath,
  getAdminSellersPath,
  getAdminTagsPath,
  getAdminUsersPath,
} from '~/routing';

import { MenuItem } from './types';

type UseAdminMenu = () => MenuItem[];

const useAdminMenu: UseAdminMenu = () => {
  const { data } = useMyUserProfile();
  const { roles } = useConfig();

  return useMemo(() => {
    if (!data?.roles?.includes(roles.admin)) {
      return [];
    }

    return [
      { id: 'admin', title: 'Администратор' },
      { icon: PlaylistAddCheck, id: 'sellers management', title: 'Управление продавцами', to: getAdminSellersPath() },
      { icon: PeopleOutline, id: 'users management', title: 'Управление пользователями', to: getAdminUsersPath() },
      { icon: ListAlt, id: 'products management', title: 'Управление товарами', to: getAdminProductsPath() },
      { icon: ListAlt, id: 'collections management', title: 'Управление коллекциями', to: getAdminCollectionsPath() },
      { icon: ListAlt, id: 'tags management', title: 'Управление тегами', to: getAdminTagsPath() },
      { icon: ListAlt, id: 'global management', title: 'Глобальные настройки', to: getAdminGlobalPath() },
    ];
  }, [data?.roles, roles.admin]);
};

export { useAdminMenu };
export type { UseAdminMenu };
