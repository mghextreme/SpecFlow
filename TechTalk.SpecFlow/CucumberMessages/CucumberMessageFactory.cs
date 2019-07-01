﻿using System;
using Google.Protobuf.WellKnownTypes;
using Io.Cucumber.Messages;
using TechTalk.SpecFlow.CommonModels;

namespace TechTalk.SpecFlow.CucumberMessages
{
    public class CucumberMessageFactory : ICucumberMessageFactory
    {
        private const string UsedCucumberImplementationString = @"SpecFlow";

        public string ConvertToPickleIdString(Guid id)
        {
            return $"{id:D}";
        }

        public IResult<TestRunStarted> BuildTestRunStartedMessage(DateTime timeStamp)
        {
            if (timeStamp.Kind != DateTimeKind.Utc)
            {
                return Result<TestRunStarted>.Failure($"{nameof(timeStamp)} must be an UTC {nameof(DateTime)}. It is {timeStamp.Kind}");
            }

            var testRunStarted = new TestRunStarted
            {
                Timestamp = Timestamp.FromDateTime(timeStamp),
                CucumberImplementation = UsedCucumberImplementationString
            };

            return Result<TestRunStarted>.Success(testRunStarted);
        }

        public IResult<TestCaseStarted> BuildTestCaseStartedMessage(Guid pickleId, DateTime timeStamp)
        {
            if (timeStamp.Kind != DateTimeKind.Utc)
            {
                return Result<TestCaseStarted>.Failure($"{nameof(timeStamp)} must be an UTC {nameof(DateTime)}. It is {timeStamp.Kind}");
            }

            var testCaseStarted = new TestCaseStarted
            {
                Timestamp = Timestamp.FromDateTime(timeStamp),
                PickleId = ConvertToPickleIdString(pickleId)
            };

            return Result<TestCaseStarted>.Success(testCaseStarted);
        }

        public IResult<TestCaseFinished> BuildTestCaseFinishedMessage(Guid pickleId, DateTime timeStamp, TestResult testResult)
        {
            if (testResult is null)
            {
                return Result<TestCaseFinished>.Failure(new ArgumentNullException(nameof(testResult)));
            }

            if (timeStamp.Kind != DateTimeKind.Utc)
            {
                return Result<TestCaseFinished>.Failure($"{nameof(timeStamp)} must be an UTC {nameof(DateTime)}. It is {timeStamp.Kind}");
            }

            var testCaseFinished = new TestCaseFinished
            {
                PickleId = ConvertToPickleIdString(pickleId),
                Timestamp = Timestamp.FromDateTime(timeStamp),
                TestResult = testResult
            };

            return Result<TestCaseFinished>.Success(testCaseFinished);
        }

        public IResult<Wrapper> BuildWrapperMessage(IResult<TestRunStarted> testRunStarted)
        {
            switch (testRunStarted)
            {
                case ISuccess<TestRunStarted> success:
                    return Result<Wrapper>.Success(new Wrapper { TestRunStarted = success.Result });
                case IFailure failure:
                    return Result<Wrapper>.Failure($"{nameof(testRunStarted)} must be an {nameof(ISuccess<TestRunStarted>)}.", failure);
                default:
                    return Result<Wrapper>.Failure($"{nameof(testRunStarted)} must be an  {nameof(ISuccess<TestRunStarted>)}.");
            }
        }

        public IResult<Wrapper> BuildWrapperMessage(IResult<TestCaseStarted> testCaseStarted)
        {
            switch (testCaseStarted)
            {
                case ISuccess<TestCaseStarted> success:
                    return Result<Wrapper>.Success(new Wrapper { TestCaseStarted = success.Result });
                case IFailure failure:
                    return Result<Wrapper>.Failure($"{nameof(testCaseStarted)} must be an {nameof(ISuccess<TestCaseStarted>)}.", failure);
                default:
                    return Result<Wrapper>.Failure($"{nameof(testCaseStarted)} must be an {nameof(ISuccess<TestCaseStarted>)}.");
            }
        }

        public IResult<Wrapper> BuildWrapperMessage(IResult<TestCaseFinished> testCaseFinished)
        {
            switch (testCaseFinished)
            {
                case ISuccess<TestCaseFinished> success:
                    return Result<Wrapper>.Success(new Wrapper { TestCaseFinished = success.Result });
                case IFailure failure:
                    return Result<Wrapper>.Failure($"{nameof(testCaseFinished)} must be an {nameof(ISuccess<TestCaseStarted>)}.", failure);
                default:
                    return Result<Wrapper>.Failure($"{nameof(testCaseFinished)} must be an {nameof(ISuccess<TestCaseStarted>)}.");
            }
        }
    }
}