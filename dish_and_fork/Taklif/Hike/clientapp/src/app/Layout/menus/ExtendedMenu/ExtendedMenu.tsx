import { Box } from '@mui/material';
import { FC } from 'react';

import { Sup } from '../Sup';
import { StyledA, StyledLink } from './StyledLink';
import { useItems } from './useItems';

const ExtendedMenu: FC = () => {
  const items = useItems();

  return (
    <>
      {items.map((item) => {
        if (!('props' in item)) {
          if (!item.to) {
            return (
              <Box key={item.id} bgcolor="background.default" pb={1} pt={2} px={2}>
                {item.title}
              </Box>
            );
          }

          if (item.external) {
            return (
              <StyledA target={item.target} href={item.to}>
                {item.title}
              </StyledA>
            );
          }

          const Icon = item.icon;

          return (
            <StyledLink key={item.id} to={item.to} href="default">
              {Icon && <Icon />}{' '}
              <span>
                {item.title} {item.sup && <Sup>{item.sup}</Sup>}
              </span>
            </StyledLink>
          );
        }
        return <>{item}</>;
      })}
    </>
  );
};
export { ExtendedMenu };
