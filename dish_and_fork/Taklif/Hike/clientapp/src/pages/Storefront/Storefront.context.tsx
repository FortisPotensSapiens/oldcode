import React from 'react';

import { CollectionReadModel } from '~/types/collection';

type StorefrontContextType = {
  selectedCollection: undefined | null | CollectionReadModel;
  updateSelectedCollection: (newCollection: CollectionReadModel | undefined) => void;
  dropCollection: () => void;
};

const StorefrontContext = React.createContext<StorefrontContextType>({
  selectedCollection: null,
  // eslint-disable-next-line @typescript-eslint/no-empty-function
  dropCollection: () => {},
  // eslint-disable-next-line @typescript-eslint/no-empty-function
  updateSelectedCollection: (_v: CollectionReadModel | undefined) => {},
});

export { StorefrontContext };
