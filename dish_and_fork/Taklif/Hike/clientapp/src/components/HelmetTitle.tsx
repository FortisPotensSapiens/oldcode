import { FC } from 'react';
import { Helmet, HelmetProps } from 'react-helmet-async';

type HelmetTitleProps = HelmetProps & {
  divider?: string;
  lead?: string;
};

const HelmetTitle: FC<HelmetTitleProps> = ({ divider = '|', lead = 'Dish & Fork', title, ...props }) => (
  <Helmet {...props} title={title ? `${lead} ${divider} ${title}` : lead} />
);

export type { HelmetTitleProps };
export { HelmetTitle };
