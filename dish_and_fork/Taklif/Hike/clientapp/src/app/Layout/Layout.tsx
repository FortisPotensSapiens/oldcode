import styled from '@emotion/styled';
import { Box, Stack } from '@mui/material';
import { FC, useCallback, useMemo, useState } from 'react';
import { Outlet } from 'react-router-dom';

import { SmBox, WrapperBox } from '~/components';
import config from '~/config';
import { useConfig } from '~/contexts';
import { useDownSm, useIsLogged, useMyUserProfile } from '~/hooks';

import { AuthButton } from './AuthButton';
import { Boundry } from './Boundry';
import { CartButton } from './CartButton';
import { Contacts } from './Contacts/Contacts';
import { DocumentsListContainer } from './DocumentsList/DocumentsListContainer';
import { LayoutContext } from './Layout.context';
import { FooterMenu, ProfileMenu, TopMenu } from './menus';
import { SellerButton } from './SellerButton';
import { SellerNotifications } from './SellerNotifications/SellerNotifications';
import { Support } from './Support';

type LayoutProps = {
  noSmallFooter?: boolean;
  noSmallHeader?: boolean;
};

const StyledTestMode = styled.div({
  position: 'sticky',
  top: '0px',
  alignSelf: 'flex-start',
  zIndex: 999,
  width: '100%',
  padding: '10px',
  backgroundColor: 'rgb(126, 0, 74, 0.7)',
  fontSize: '90%',
  color: 'white',
  borderBottom: '1px solid white',
});

const PROFILE_TEXT = 'Профиль';

const Layout: FC<LayoutProps> = ({ children, noSmallFooter, noSmallHeader }) => {
  const isSmall = useDownSm();
  const config = useConfig();
  const isLogged = useIsLogged();
  const [showTest, updateShowTest] = useState(true);
  const { data: my, isFetched } = useMyUserProfile();

  const showTestLabel = useCallback(() => {
    updateShowTest(true);
  }, [updateShowTest]);

  const hideTestLabel = useCallback(() => {
    updateShowTest(false);
  }, [updateShowTest]);

  const layoutContextValue = useMemo(() => {
    return {
      hideTestLabel,
      showTestLabel,
    };
  }, [hideTestLabel, showTestLabel]);

  const isSellerNotAdmin = useMemo(() => {
    return isFetched && my?.roles?.includes(config.roles.seller) && !my?.roles?.includes(config.roles.admin);
  }, [config.roles.admin, config.roles.seller, isFetched, my?.roles]);

  return (
    <LayoutContext.Provider value={layoutContextValue}>
      <Box minHeight="100%" alignItems="stretch" display="flex" flexDirection="column">
        <Boundry hideSmall={noSmallHeader} alignItems="center">
          <TopMenu flexGrow={1} hideSmall ml={3} mr={2} />
          <SmBox hideSmall>
            <Stack alignItems="center" direction="row" spacing={2}>
              <SellerButton />
              {isLogged ? <ProfileMenu text={PROFILE_TEXT} /> : <AuthButton text={PROFILE_TEXT} />}
              <CartButton />
            </Stack>
          </SmBox>
        </Boundry>

        {isLogged && isSellerNotAdmin ? <SellerNotifications /> : undefined}
        {config.isTestingMode && showTest ? (
          <StyledTestMode>Сайт находится в тестовом режиме, заказ товаров временно недоступен.</StyledTestMode>
        ) : undefined}

        <Box
          bgcolor="background.default"
          display="flex"
          flexGrow={1}
          justifyContent="center"
          pb={{ sm: 0, xs: noSmallFooter ? 0 : 7 }}
          zIndex={1}
        >
          <WrapperBox display="block">{children || <Outlet />}</WrapperBox>
        </Box>

        {isSmall && !noSmallFooter && <FooterMenu />}

        <Boundry footer hideSmall zIndex={99}>
          <DocumentsListContainer />
          <Contacts />
        </Boundry>
        <Support />
      </Box>
    </LayoutContext.Provider>
  );
};

export { Layout };
export type { LayoutProps };
