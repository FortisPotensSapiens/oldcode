import { PageLayout } from '~/components';

import { CollectionsListContainer } from './CollectionsList/CollectionsListContainer';

const AdminCollections = () => {
  return (
    <PageLayout title="Управление коллекциями">
      <CollectionsListContainer />
    </PageLayout>
  );
};

export { AdminCollections };
