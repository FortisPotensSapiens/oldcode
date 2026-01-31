import { FC } from 'react';

import { ReactComponent as Logo } from '~/assets/images/logo.svg';
import { SmBox, SmBoxProps, WrapperBox } from '~/components';

import StyledLink from './StyledLink';

type BoundryProps = SmBoxProps & { footer?: true };

const Boundry: FC<BoundryProps> = ({ children, footer, alignItems, ...props }) => {
  return (
    <SmBox
      bgcolor="common.white"
      borderBottom={footer ? 0 : 1}
      borderTop={footer ? 1 : 0}
      borderColor="grey.100" // eslint-disable-line react/jsx-sort-props
      component={footer ? 'footer' : 'header'}
      flexGrow={0}
      flexShrink={0}
      justifyContent="center"
      paddingTop="0.5rem"
      paddingBottom="0.5rem"
      width={1}
      zIndex={2}
      {...props}
    >
      <WrapperBox justifyContent={{ sm: 'space-between', xs: 'center' }} alignItems={alignItems ?? 'flex-start'}>
        <StyledLink to="/">
          <Logo height="100%" />
        </StyledLink>

        {children}
      </WrapperBox>
    </SmBox>
  );
};

export type { BoundryProps };
export { Boundry };
