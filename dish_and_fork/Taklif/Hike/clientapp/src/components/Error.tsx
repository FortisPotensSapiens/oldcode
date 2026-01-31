import { FC } from 'react';

import { EmptyList } from './EmptyList';
import { PageLayout, PageLayoutProps } from './PageLayout';

const Error: FC<Partial<PageLayoutProps>> = ({ children, title = 'Ошибка' }) => (
  <PageLayout title={title}>
    <EmptyList>{children}</EmptyList>
  </PageLayout>
);

export { Error };
