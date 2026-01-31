import { useOidc } from '@axa-fr/react-oidc';
import { useCallback, useState } from 'react';
import { v4 as uuidv4 } from 'uuid';

import AddFeedback from './AddFeedback/AddFeedback';
import FeedbackListContainer from './FeedbackList/FeedbackListContainer';

const FeedbackTab = ({ productId }: { productId: string }) => {
  const { isAuthenticated } = useOidc();
  const [listId, setListId] = useState(uuidv4());

  const onSuccessUpdateCallback = useCallback(() => {
    setListId(uuidv4());
  }, []);

  return (
    <>
      <AddFeedback productId={productId} isAuth={isAuthenticated} onSuccess={onSuccessUpdateCallback} />

      <FeedbackListContainer productId={productId} key={listId} />
    </>
  );
};

export default FeedbackTab;
