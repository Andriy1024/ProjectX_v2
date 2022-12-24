export interface IResponse {
    isFailed: boolean,
    error: IError
}

export interface IDataResponseOf<T> extends IResponse {
    data: T
}

export interface IError {
    message: string,
    type: number,
    code: number
}

export const mapResponseOf = <TOut>(response: IDataResponseOf<TOut>): TOut => {
    if (response.isFailed) {
        console.error('ResponseOf<> Error ' + response.error);
        throw new Error(`ResponseOf<> Error: ${response?.error?.message}`);
    }
    return response.data;
};

export const mapResponse = (response: IResponse): IResponse => {
    if (response.isFailed) {
        console.error('Response error:', response.error);
        throw new Error(`Response Error, error: ${response?.error?.message}`);
    }
    return response;
};
