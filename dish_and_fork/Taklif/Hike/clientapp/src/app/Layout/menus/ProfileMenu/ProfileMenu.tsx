import { Logout, PersonOutline } from '@mui/icons-material';
import { Box, MenuItem } from '@mui/material';
import { bindMenu, bindTrigger, usePopupState } from 'material-ui-popup-state/hooks';
import { FC, ReactNode } from 'react';

import { MenuButton } from '~/components';
import { useLogout } from '~/hooks';

import { Sup } from '../Sup';
import { StyledLink } from './StyledLink';
import { StyledMenu } from './StyledMenu';
import { useItems } from './useItems';

type ProfileMenuProps = { text?: ReactNode };

const ProfileMenu: FC<ProfileMenuProps> = ({ text }) => {
  const logout = useLogout();
  const items = useItems();
  const popupState = usePopupState({ popupId: 'profile-menu', variant: 'popover' });
  const { close } = popupState;

  return (
    <>
      <MenuButton icon={<PersonOutline />} {...bindTrigger(popupState)}>
        {text}
      </MenuButton>

      <StyledMenu {...bindMenu(popupState)}>
        {items.map(({ icon: Icon, id, sup, title, to }) => {
          if (!to) {
            return (
              <MenuItem key={id} disabled>
                {title}
              </MenuItem>
            );
          }

          return (
            <MenuItem key={id} onClick={close}>
              <StyledLink to={to}>
                {({ isActive }) => {
                  const content = (
                    <>
                      {Icon && <Icon />}

                      <span>
                        {title} {sup && <Sup>{sup}</Sup>}
                      </span>
                    </>
                  );

                  return isActive ? (
                    <Box alignItems="center" color="primary.main" component="span" display="flex">
                      {content}
                    </Box>
                  ) : (
                    content
                  );
                }}
              </StyledLink>
            </MenuItem>
          );
        })}

        <MenuItem onClick={logout}>
          <StyledLink end onClick={(event) => event.preventDefault()} to="#">
            <Logout /> Выйти
          </StyledLink>
        </MenuItem>
      </StyledMenu>
    </>
  );
};

export { ProfileMenu };
export type { ProfileMenuProps };
