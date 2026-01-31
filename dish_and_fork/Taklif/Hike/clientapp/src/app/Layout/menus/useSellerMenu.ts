import { FiberNew, ListAlt, LocalOfferOutlined, OtherHousesOutlined } from '@mui/icons-material';
import { useMemo } from 'react';

import { ReactComponent as Cake } from '~/assets/icons/cake.svg';
import { useConfig } from '~/contexts';
import { useMyUserProfile } from '~/hooks';
import {
  getPartnerNewAppsPath,
  getPartnerOffersPath,
  getPartnerOrdersPath,
  getPartnerProductsPath,
  getPartnerSettingsPath,
} from '~/routing';

import { MenuItem } from './types';

type UseSellerMenu = () => MenuItem[];

const useSellerMenu: UseSellerMenu = () => {
  const { data } = useMyUserProfile();
  const { roles } = useConfig();

  return useMemo(() => {
    if (!data?.roles?.includes(roles.seller)) {
      return [];
    }

    return [
      { id: 'seller', title: 'Мой магазин' },
      { icon: OtherHousesOutlined, id: 'my shop', title: 'Настройки магазина', to: getPartnerSettingsPath() },
      { icon: Cake, id: 'my products', title: 'Мои товары', to: getPartnerProductsPath() },
      { icon: ListAlt, id: 'my sells', title: 'Мои продажи', to: getPartnerOrdersPath() },
      { icon: FiberNew, id: 'new apps', title: 'Доступные заказы', to: getPartnerNewAppsPath() },
      { icon: LocalOfferOutlined, id: 'my offers', title: 'Мои отклики', to: getPartnerOffersPath() },
    ];
  }, [data?.roles, roles.seller]);
};

export { useSellerMenu };
export type { UseSellerMenu };
