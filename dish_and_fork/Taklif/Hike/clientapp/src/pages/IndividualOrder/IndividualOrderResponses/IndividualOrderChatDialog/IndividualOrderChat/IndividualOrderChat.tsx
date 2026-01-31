import { useOidcAccessToken } from '@axa-fr/react-oidc';
import * as signalR from '@microsoft/signalr';
import { HubConnection } from '@microsoft/signalr';
import { Box, List, ListItem, styled } from '@mui/material';
import _ from 'lodash-es';
import { useCallback, useEffect, useRef, useState } from 'react';

import { useGetIndividualOrderOfferInfo } from '~/api/v1/useGetIndividualOrderOfferInfo';
import { LoadingSpinner } from '~/components';
import config from '~/config';
import { OfferCommentReadModel } from '~/types';

import { AddCommentForm } from '../AddCommentForm';
import IndividualOrderChatComment from '../IndividualOrderChatComment/IndividualOrderChatComment';

const StyledFooter = styled(Box)(({ theme }) => ({
  paddingTop: theme.spacing(2),
}));

const StyledContent = styled(Box)<{ chatBodyHeight: string | undefined }>(({ theme, chatBodyHeight }) => ({
  flexGrow: 1,
  overflowY: 'scroll',
  maxHeight: chatBodyHeight ?? 'auto',
}));

const IndividualOrderChat = ({ offerId, chatBodyHeight }: { offerId: string; chatBodyHeight?: string }) => {
  const accessToken = useOidcAccessToken();
  const { data, isLoading } = useGetIndividualOrderOfferInfo(String(offerId));
  const [comments, setComments] = useState<OfferCommentReadModel[]>([]);
  const [commentIds, setCommentIds] = useState<Record<string, boolean>>({});
  const connectionRef = useRef<HubConnection | null>(null);
  const chatBotContainer = useRef<any>(null);

  useEffect(() => {
    setComments([...comments, ...(data?.comments ?? [])]);
  }, [data?.comments]);

  useEffect(() => {
    setCommentIds(
      comments.reduce((prev, item) => {
        prev[item.id] = true;

        return prev;
      }, {} as Record<string, boolean>),
    );

    scrollDown();
  }, [comments]);

  useEffect(() => {
    const connection = new signalR.HubConnectionBuilder()
      .withUrl(config.hubs.baseUrl, {
        accessTokenFactory: () => {
          return accessToken.accessToken;
        },
      })
      .withAutomaticReconnect([0, 0, 10000])
      .configureLogging(signalR.LogLevel.Information)
      .build();

    try {
      connection.start();
      console.log('SignalR Connected.');
    } catch (err) {
      console.log(err);
    }

    connectionRef.current = connection;

    return () => {
      connection.stop();
    };
  }, []);

  const scrollDown = useCallback(() => {
    if (chatBotContainer.current) {
      chatBotContainer.current.scrollTo(0, chatBotContainer.current.scrollHeight);
    }
  }, [chatBotContainer]);

  useEffect(() => {
    connectionRef.current?.off('OfferCommentAdded');
    connectionRef.current?.on('OfferCommentAdded', (data) => {
      const newComment = data as OfferCommentReadModel;

      const existComment = commentIds[data.id];

      if (!existComment) {
        setComments([...comments, newComment]);
      }
    });
  }, [comments, commentIds, connectionRef]);

  return (
    <>
      {isLoading ? (
        <LoadingSpinner />
      ) : (
        <StyledContent chatBodyHeight={chatBodyHeight} ref={chatBotContainer}>
          <List>
            {comments.map((comment) => {
              return (
                <ListItem key={comment.id}>
                  <IndividualOrderChatComment comment={comment} />
                </ListItem>
              );
            })}
          </List>
        </StyledContent>
      )}
      {offerId ? (
        <StyledFooter>
          <AddCommentForm offerId={offerId} />
        </StyledFooter>
      ) : null}
    </>
  );
};

export default IndividualOrderChat;
