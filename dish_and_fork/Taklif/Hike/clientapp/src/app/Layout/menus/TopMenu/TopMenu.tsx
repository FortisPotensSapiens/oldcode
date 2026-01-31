import { FC } from 'react';

import { SmBox, SmBoxProps } from '~/components';
import { useSupportActions } from '~/contexts';
import { useDownMd } from '~/hooks';
import { getAboutPath, getSellersPath } from '~/routing';

import { individualAppItem as custom } from '../customMenu';
import { Sup } from '../Sup';
import { MenuLink } from './MenuLink';
import { StyledLink } from './StyledLink';

type TopMenuProps = SmBoxProps;

const TopMenu: FC<TopMenuProps> = (props) => {
  const { show } = useSupportActions();
  const isSm = useDownMd();

  return (
    <SmBox margin={0} padding={0} typography="body1" {...props}>
      <MenuLink to={getSellersPath()}>Список продавцов</MenuLink>

      <StyledLink
        end
        onClick={(event) => {
          event.preventDefault();
          show();
        }}
        to="#"
      >
        Поддержка
      </StyledLink>

      {!isSm && (
        <MenuLink to={custom.to}>
          {custom.title} {custom.sup && <Sup>{custom.sup}</Sup>}
        </MenuLink>
      )}
    </SmBox>
  );
};

export { TopMenu };
export type { TopMenuProps };
