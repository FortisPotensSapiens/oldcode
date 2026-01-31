import { StarPurple500 } from '@mui/icons-material';
import MoreHorizIcon from '@mui/icons-material/MoreHoriz';
import { Box, SvgIcon, Typography } from '@mui/material';
import * as bodyLock from 'body-scroll-lock';
import { FC, useContext, useEffect, useRef } from 'react';
import { useLocation } from 'react-router-dom';

import { ReactComponent as BasketIcon } from '~/assets/icons/basket.svg';
import { ReactComponent as HomeIcon } from '~/assets/icons/home.svg';
import { useCartCount } from '~/hooks';
import { getCartPath, getHomepagePath, getIndividualOrdersPath } from '~/routing';

import { Contacts } from '../../Contacts/Contacts';
import { LayoutContext } from '../../Layout.context';
import { ExtendedMenu } from '../ExtendedMenu';
import { StyledBadge } from './StyledBadge';
import { StyledContainer, StyledContainerProps } from './StyledContainer';
import { StyledItem } from './StyledItem';
import { ContactsWrapper, StyledWrapper, StyledWrapperInside } from './StyledWrapper';
import { useBodyOverflow } from './useBodyOverflow';
import { useGoTo } from './useGoTo';
import { useMenuState } from './useMenuState';

type FooterMenuProps = StyledContainerProps;

const homepagePath = getHomepagePath();
const cartPath = getCartPath();
const individualPath = getIndividualOrdersPath();

const FooterMenu: FC<FooterMenuProps> = ({ children }) => {
  const location = useLocation();
  const [menu, { show }] = useMenuState();
  const ref = useRef(null);
  const popupRef = useRef(null);
  const count = useCartCount();
  const goTo = useGoTo();
  const { pathname } = location;
  const layout = useContext(LayoutContext);

  useBodyOverflow(menu);

  useEffect(() => {
    if (menu && popupRef.current) {
      bodyLock.disableBodyScroll(popupRef.current);
    }

    if (!menu) {
      bodyLock.clearAllBodyScrollLocks();
    }
  }, [menu]);

  useEffect(() => {
    if (menu) {
      layout.hideTestLabel();
    } else {
      layout.showTestLabel();
    }
  }, [layout, menu]);

  return (
    <>
      {menu && (
        <StyledWrapper ref={popupRef}>
          <StyledWrapperInside>
            <Box bgcolor="background.paper" component={Typography} pb={2} pt={2} textAlign="center" variant="h5">
              Другое
            </Box>

            <Box bgcolor="background.paper" position="relative" top={1}>
              <ExtendedMenu />
            </Box>
            <ContactsWrapper>
              <Contacts />
            </ContactsWrapper>
          </StyledWrapperInside>
        </StyledWrapper>
      )}

      <StyledContainer>
        <StyledItem active={!menu && pathname === homepagePath} data-href={homepagePath} onClick={goTo}>
          <SvgIcon component={HomeIcon} />
          <Typography variant="caption">Главная</Typography>
        </StyledItem>

        <StyledItem active={!menu && pathname === cartPath} data-href={cartPath} onClick={goTo}>
          <StyledBadge badgeContent={count || undefined} color="primary">
            <SvgIcon component={BasketIcon} />
          </StyledBadge>

          <Typography variant="caption">Корзина</Typography>
        </StyledItem>

        <StyledItem active={!menu && pathname === individualPath} data-href={individualPath} onClick={goTo}>
          <StyledBadge color="primary">
            <SvgIcon component={StarPurple500} />
          </StyledBadge>

          <Typography variant="caption">Индивидуальный</Typography>
        </StyledItem>

        <StyledItem ref={ref} active={menu} onClick={show}>
          <MoreHorizIcon />
          <Typography variant="caption">Еще</Typography>
        </StyledItem>
      </StyledContainer>
    </>
  );
};

export { FooterMenu };
export type { FooterMenuProps };
