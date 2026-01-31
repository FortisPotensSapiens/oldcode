import { Button, Divider, Spin, Tooltip } from 'antd';
import Title from 'antd/es/typography/Title';
import { useCallback } from 'react';

import { useGetCanISetFeedbackPartnerById } from '~/api/v1/useGetCanISetFeedbackPartnerById';
import { useGetMyPartnerRating } from '~/api/v1/useGetMyPartnerRating';

import AddFeedbackFormAdd from './AddFeedbackFormAdd';

const AddFeedback = ({
  productId,
  isAuth,
  onSuccess,
}: {
  productId: string;
  isAuth: boolean;
  onSuccess: () => void;
}) => {
  const { isLoading, data } = useGetCanISetFeedbackPartnerById(productId);
  const {
    isLoading: isLoadingMyRating,
    data: myRatingData,
    refetch: refreshMyRating,
  } = useGetMyPartnerRating(productId);

  const onSuccessEditCallback = useCallback(() => {
    refreshMyRating();
    onSuccess();
  }, [onSuccess, refreshMyRating]);

  const onSuccessAddCallback = useCallback(() => {
    console.log('succes');
    onSuccess();
  }, [onSuccess]);

  if (isLoading || isLoadingMyRating) {
    return <Spin />;
  }

  return (
    <>
      {isAuth ? (
        <>
          {data ? (
            myRatingData?.id ? (
              <Title level={5} style={{ textAlign: 'center' }}>
                Спасибо за ваш отзыв.
              </Title>
            ) : (
              <AddFeedbackFormAdd productId={productId} onSuccess={onSuccessAddCallback} />
            )
          ) : (
            <>
              <Tooltip placement="topLeft" title="Для того что бы оставить отзыв сделайте пожалуйста заказ">
                <Button type="primary" disabled>
                  Оставить отзыв
                </Button>
              </Tooltip>
            </>
          )}
          <Divider orientation="center" />
        </>
      ) : undefined}
    </>
  );
};

export default AddFeedback;
