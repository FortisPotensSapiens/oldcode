import { InfoOutlined, PlaylistAddCheck, Support } from '@mui/icons-material';
import { useMemo } from 'react';

import { ReactComponent as Cook } from '~/assets/icons/cook.svg';
import { useSupportActions } from '~/contexts';
import { useCanBeSeller } from '~/hooks';
import { getAboutPath, getBecomeSellerPath, getSellersPath } from '~/routing';

import { individualAppItem } from '../customMenu';
import { MenuItem } from '../types';
import { useAdminMenu } from '../useAdminMenu';
import { useCommonMenu } from '../useCommonMenu';
import { useDocumentsMenu } from '../useDocumentsMenu';
import { useSellerMenu } from '../useSellerMenu';
import { LoginLogout } from './LoginLogout';
import { StyledLink } from './StyledLink';

type UseItems = () => (MenuItem | JSX.Element)[];

const useItems: UseItems = () => {
  const commonMenu = useCommonMenu();
  const sellerMenu = useSellerMenu();
  const adminMenu = useAdminMenu();
  const canBeSeller = useCanBeSeller();
  const documentsMenu = useDocumentsMenu();
  const { show } = useSupportActions();

  return useMemo(() => {
    const become: MenuItem[] = canBeSeller
      ? [{ icon: Cook, id: 'become seller', title: 'Стать продавцом', to: getBecomeSellerPath() }]
      : [];

    const result: (MenuItem | JSX.Element)[] = [
      { id: 'main', title: 'Основное' },
      ...become,
      { icon: PlaylistAddCheck, id: 'sellers list', title: 'Список продавцов', to: getSellersPath() },

      ...commonMenu,
      ...sellerMenu,

      ...adminMenu,

      { id: 'other', title: 'Остальное' },

      <>
        <StyledLink
          onClick={(event) => {
            event.preventDefault();
            show();
          }}
          to="#"
        >
          <Support /> Поддержка
        </StyledLink>

        <LoginLogout />
      </>,

      { id: 'documents', title: 'Документы' },

      ...documentsMenu,
    ];

    return result;
  }, [adminMenu, commonMenu, sellerMenu, canBeSeller]);
};

export { useItems };
export type { UseItems };
